import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-tobuy-edit',
  templateUrl: './tobuy-edit.component.html',
  styleUrls: ['./tobuy-edit.component.css']
})
export class TobuyEditComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
  ) {

  }

  ngOnInit() {
    const id = +this.route.snapshot.paramMap.get('id');
  }

}
