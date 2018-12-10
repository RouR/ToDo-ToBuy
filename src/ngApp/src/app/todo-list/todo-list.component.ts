import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { Observable, BehaviorSubject, of, merge, fromEvent } from 'rxjs';
import { TodoPublicEntity, Client, DeleteTODORequest } from 'src/_tsModels/api-client';
import { map, catchError, finalize, tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { MatPaginator, MatSort, MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { ConfirmationService } from '../_services/confirmation.service';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit, AfterViewInit {

  dataSource: TodoDataSource;
  displayedColumns = ['title', 'description', 'updated', 'buttons'];

  pageSize = 5;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;

  constructor(
    private api: Client,
    private router: Router,
    private snackBar: MatSnackBar,
    private confirmService: ConfirmationService
  ) { }

  ngOnInit() {
    this.dataSource = new TodoDataSource(this.api);
    this.dataSource.findTodoItems('', '', 'asc', 0, 5);
  }

  ngAfterViewInit() {

    // server-side search
    fromEvent(this.input.nativeElement, 'keyup')
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = 0;
          this.load();
        })
      )
      .subscribe();

    // reset the paginator after sorting
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap(() => this.load())
      )
      .subscribe();
  }

  private load(): void {
    return this.dataSource
      .findTodoItems(this.input.nativeElement.value,
        this.sort.active, this.sort.direction,
        this.paginator.pageIndex, this.paginator.pageSize);
  }

  onRowClicked(row: TodoPublicEntity) {
    // console.log('Row clicked: ', row);
    // this.router.navigate(['/todo', row.publicId]);
  }

  delete(todoItem: TodoPublicEntity, event: MouseEvent) {
    event.stopPropagation();
    this.confirmService.confirm(todoItem.title || todoItem.description, 'Are you sure you want to permanently delete it?')
      .subscribe(
        result => {
          if (!result) {
            // console.log('delete is cancelled', todoItem);
            this.snackBar.open('Delete is cancelled', 'Delete did not happen.', {
              duration: 2500,
              horizontalPosition: 'right',
              verticalPosition: 'bottom',
            });
          } else {
            // console.log('ready to delete', todoItem);
            const request = new DeleteTODORequest({
              publicId: todoItem.publicId
            });
            this.api.apiTodoDelete(request).subscribe(
              resultApi => {
                // console.log('deleted', resultApi);
                if (!resultApi.hasError) {
                  this.load();
                  if (resultApi.data) {
                    // this.load();
                  } else {
                    this.snackBar.open('Unknown reason at server', 'Delete did not happen.', {
                      duration: 2500,
                      horizontalPosition: 'right',
                      verticalPosition: 'bottom',
                    });
                  }
                }
              },
              err => {
                console.log(err.error);
                console.log(err.message);
                this.snackBar.open(err.message, 'Delete did not happen.', {
                  duration: 2500,
                  horizontalPosition: 'right',
                  verticalPosition: 'bottom',
                });
              }
            );
          }
        }
      );
  }
}

class TodoDataSource implements DataSource<TodoPublicEntity> {

  private lessonsSubject = new BehaviorSubject<TodoPublicEntity[]>([]);

  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loading$ = this.loadingSubject.asObservable();

  private totalRecords = new BehaviorSubject<number>(0);
  public totalRecords$ = this.totalRecords.asObservable();

  constructor(private api: Client) { }

  connect(collectionViewer: CollectionViewer): Observable<TodoPublicEntity[]> {
    return this.lessonsSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    this.lessonsSubject.complete();
    this.loadingSubject.complete();
  }

  findTodoItems(filter = '', sortField = '', sortDirection = 'asc', pageIndex = 0, pageSize = 3) {
    this.loadingSubject.next(true);
    this.api
      .apiTodoList(pageIndex + 1, pageSize, sortField, sortDirection === 'asc', filter)
      .pipe(
        map(res => {
          this.totalRecords.next(res.totalItems);
          return res.items;
        }
        ),
        catchError(() => {
          this.totalRecords.next(0);
          return of([]);
        }),
        finalize(() => this.loadingSubject.next(false))
      )
      .subscribe(lessons => this.lessonsSubject.next(lessons));
  }
}
