import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TOBUYPublicEntity, Client, SaveTOBUYRequest, PriceCurrency, Price } from 'src/_tsModels/api-client';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DATE_FORMATS } from '@angular/material';
import { mergeMap, catchError } from 'rxjs/operators';
import { Observable, throwError, of } from 'rxjs';

// See the Moment.js docs for the meaning of these formats:
// https://momentjs.com/docs/#/displaying/format/
export const MY_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-tobuy-edit',
  templateUrl: './tobuy-edit.component.html',
  styleUrls: ['./tobuy-edit.component.css'],
  providers: [
    {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},
  ],
})
export class TobuyEditComponent implements OnInit {
  id: string;
  isNewRecord: boolean;
  isLoading: boolean;

  Currency = PriceCurrency;
  AvailableCurrencies: number[];

  tbForm = this.fb.group({
    publicId: [null],
    name: [null, Validators.required],
    qty: [null],
    dueToUtc: [null],
    price: this.fb.group({
      amount: [null],
      currency: [null]
    }),
  });
  minDate = new Date();

  constructor(
    private route: ActivatedRoute,
    private api: Client,
    private fb: FormBuilder,
  ) {
    this.AvailableCurrencies = Object.keys(this.Currency)
          .map(x => isNaN(Number(x)) ? -1 : Number(x))
          .filter(f => f > 0);
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.isLoading = false;
    if (!this.id) {
      this.isNewRecord = true;
    } else {
      this.isLoading = true;
      this.api.apiTobuyGet(this.id)
      .subscribe(
        result => {
          this.tbForm.patchValue(result.data);
        },
        error => { },
        () => {
          this.isLoading = false;
        }
      );
    }
  }

  save() {
    if (this.isLoading) {
      return;
    }

    if (!this.tbForm.valid) {
      return;
    }

    const req = new SaveTOBUYRequest({
      publicId: this.isNewRecord ? null : this.tbForm.value.publicId,
      name: this.tbForm.value.name,
      qty: this.tbForm.value.qty,
      price: new Price({
        amount: this.tbForm.value.price.amount,
        currency: this.tbForm.value.price.currency,
      }),
      dueToUtc: this.tbForm.value.dueToUtc,
    });
    if (this.isNewRecord) {
      this.createFunc(req);
    } else {
      this.saveFunc(req);
    }
  }

  createFunc(data: SaveTOBUYRequest) {
    this.isLoading = true;
    this.api.apiTobuyCreate(data).subscribe(
      result => {
        if (!result.hasError) {
          this.tbForm.patchValue(result.data);
          this.isNewRecord = false;
        }
      },
      error => { },
      () => {
        this.isLoading = false;
      }
    );
  }

  saveFunc(data: SaveTOBUYRequest) {
    this.isLoading = true;
    this.api.apiTobuyUpdate(data).subscribe(
      result => {
        if (!result.hasError) {
          this.tbForm.patchValue(result.data);
        }
      },
      error => { },
      () => {
        this.isLoading = false;
      }
    );
  }

}
