import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TobuyEditComponent } from './tobuy-edit.component';

describe('TobuyEditComponent', () => {
  let component: TobuyEditComponent;
  let fixture: ComponentFixture<TobuyEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TobuyEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TobuyEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
