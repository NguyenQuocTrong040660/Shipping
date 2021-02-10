import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.scss']
})
export class ConfigurationComponent implements OnInit {
  configurations: Configuration[] = [];
  selectedConfigurations: Configuration[] = [];
  isShowEditDialog: boolean;
  currentSelectedConfiguration: Configuration[] = [];
  isDeleteMany: boolean;
  configurationForm: FormGroup;
  cols: { header: string, field: string }[] = [];

  get key() {
    return this.configurationForm.get('key');
  }

  ngOnInit() {
    this.cols = [
      { header: 'Key', field: 'key' },
      { header: 'Value', field: 'value' },
      { header: 'Created', field: 'created' },
      { header: 'Create By', field: 'createBy' },
      { header: 'Last Modified', field: 'lastModified' },
      { header: 'Last Modified By', field: 'lastModifiedBy' },
    ];

    this.configurations = [
      {
        id: '1',
        key: 'MinShippingDay',
        value: '2',
        created: new Date(),
        createBy: 'Mr.A',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.A 1'
      },
      {
        id: '2',
        key: 'ShippingDeptEmail',
        value: 'shipping@gmail.com',
        created: new Date(),
        createBy: 'Mr.A',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.A 1'
      },
      {
        id: '3',
        key: 'LogisticDeptEmail',
        value: 'logistic@gmail.com',
        created: new Date(),
        createBy: 'Mr.A',
        lastModified: new Date(),
        lastModifiedBy: 'Mr.A 1'
      }
    ];

    this.configurationForm = new FormGroup({
      key: new FormControl('', Validators.required),
      value: new FormControl(''),
    });
  }

  // Edit Configuration
  openEditDialog(shippingPlan: Configuration) {
    this.isShowEditDialog = true;

    this.configurationForm.get('key').setValue(shippingPlan && shippingPlan.key);
    this.configurationForm.get('value').setValue(shippingPlan && shippingPlan.value);
  }

  hideEditDialog() {
    this.isShowEditDialog = false;
    this.configurationForm.reset();
  }

  onEdit() {
    console.log(this.configurationForm.value);

    // this.hideEditDialog();
  }
}

interface Configuration {
  id: string;
  key: string;
  value: string;
  created: Date;
  createBy: string;
  lastModified: Date;
  lastModifiedBy: string;
}
