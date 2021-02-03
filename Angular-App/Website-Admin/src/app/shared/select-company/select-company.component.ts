import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { StateService, States } from 'app/services/state.service';
import { company } from 'assets/company';

@Component({
  selector: 'app-select-company',
  templateUrl: './select-company.component.html',
  styleUrls: ['./select-company.component.scss'],
})
export class SelectCompanyComponent implements OnInit {
  @Output() EventAction = new EventEmitter<any>();
  company = company;
  companyIndex = 0;
  constructor(private stateService: StateService) {}

  ngOnInit(): void {
    this.companyIndex = this.stateService.select(States.companyIndex);
  }

  changeCompany(companyIndex: number) {
    this.stateService.setState('companyIndex', companyIndex, true);
  }
}
