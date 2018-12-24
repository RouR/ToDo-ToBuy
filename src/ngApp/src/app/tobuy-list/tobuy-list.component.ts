import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { Observable, BehaviorSubject, of, merge, fromEvent } from 'rxjs';
import { TOBUYPublicEntity, Client, DeleteTOBUYRequest, PriceCurrency, Price } from 'src/_tsModels/api-client';
import { map, catchError, finalize, tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { MatPaginator, MatSort, MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { ConfirmationService } from '../_services/confirmation.service';

@Component({
  selector: 'app-tobuy-list',
  templateUrl: './tobuy-list.component.html',
  styleUrls: ['./tobuy-list.component.css']
})
export class TobuyListComponent implements OnInit, AfterViewInit {

  dataSource: ToBuyDataSource;
  displayedColumns = ['name', 'price', 'qty', 'dueto', 'updated', 'buttons'];

  pageSize = 5;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  Currency = PriceCurrency;

  constructor(
    private api: Client,
    private router: Router,
    private snackBar: MatSnackBar,
    private confirmService: ConfirmationService
  ) { }

  ngOnInit() {
    this.dataSource = new ToBuyDataSource(this.api);
    this.dataSource.findToBuyItems('', 'asc', 0, 5);
  }

  ngAfterViewInit() {

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
      .findToBuyItems(this.sort.active, this.sort.direction,
        this.paginator.pageIndex, this.paginator.pageSize);
  }

  onRowClicked(row: TOBUYPublicEntity) {
    // console.log('Row clicked: ', row);
  }

  delete(ToBuyItem: TOBUYPublicEntity, event: MouseEvent) {
    event.stopPropagation();
    this.confirmService.confirm(ToBuyItem.name, 'Are you sure you want to permanently delete it?')
      .subscribe(
        result => {
          if (!result) {
            // console.log('delete is cancelled', ToBuyItem);
            this.snackBar.open('Delete is cancelled', 'Delete did not happen.', {
              duration: 2500,
              horizontalPosition: 'right',
              verticalPosition: 'bottom',
            });
          } else {
            // console.log('ready to delete', ToBuyItem);
            const request = new DeleteTOBUYRequest({
              publicId: ToBuyItem.publicId
            });
            this.api.apiTobuyDelete(request).subscribe(
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

  formatPrice(price: Price): string {
      if (!price) {
        return '-';
      }
      return `${price.amount} ${this.Currency[price.currency]}`;
  }
}

class ToBuyDataSource implements DataSource<TOBUYPublicEntity> {

  private lessonsSubject = new BehaviorSubject<TOBUYPublicEntity[]>([]);

  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loading$ = this.loadingSubject.asObservable();

  private totalRecords = new BehaviorSubject<number>(0);
  public totalRecords$ = this.totalRecords.asObservable();

  constructor(private api: Client) { }

  connect(collectionViewer: CollectionViewer): Observable<TOBUYPublicEntity[]> {
    return this.lessonsSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    this.lessonsSubject.complete();
    this.loadingSubject.complete();
  }

  findToBuyItems(sortField = '', sortDirection = 'asc', pageIndex = 0, pageSize = 3) {
    this.loadingSubject.next(true);
    this.api
      .apiTobuyList(pageIndex + 1, pageSize, sortField, sortDirection === 'asc')
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
