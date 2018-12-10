import { Injectable } from '@angular/core';
import { ConfirmComponent } from '../confirm/confirm.component';
import { MatDialogRef, MatDialog } from '@angular/material';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ConfirmationService {

  private dialogRef: MatDialogRef<ConfirmComponent>;

  constructor(
    private dialog: MatDialog,
  ) {
   }

  public confirm(title: string, message: string): Observable<any> {

    this.dialogRef = this.dialog.open(ConfirmComponent);
    this.dialogRef.componentInstance.title = title;
    this.dialogRef.componentInstance.message = message;

    return this.dialogRef.afterClosed();

  }
}
