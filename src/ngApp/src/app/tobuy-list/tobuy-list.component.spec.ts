import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TobuyListComponent } from './tobuy-list.component';

describe('TobuyListComponent', () => {
  let component: TobuyListComponent;
  let fixture: ComponentFixture<TobuyListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TobuyListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TobuyListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
