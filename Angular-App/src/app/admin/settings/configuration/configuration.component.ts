import { NotificationService } from 'app/shared/services/notification.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfigClients, ConfigModel } from 'app/shared/api-clients/shipping-app.client';

@Component({
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.scss'],
})
export class ConfigurationComponent implements OnInit {
  configurations: ConfigModel[] = [];
  isShowEditDialog: boolean;
  configurationForm: FormGroup;

  cols: { header: string; field: string }[] = [];
  colFields = [];

  get keyControl() {
    return this.configurationForm.get('key');
  }

  get valueControl() {
    return this.configurationForm.get('value');
  }

  constructor(private configsClients: ConfigClients, private notifiactionService: NotificationService) {}

  ngOnInit() {
    this.getConfigurations();

    this.cols = [
      { header: 'Key', field: 'key' },
      { header: 'Value', field: 'value' },
      { header: 'Created', field: 'created' },
      { header: 'Create By', field: 'createBy' },
      { header: 'Last Modified', field: 'lastModified' },
      { header: 'Last Modified By', field: 'lastModifiedBy' },
    ];

    this.colFields = this.cols.map((i) => i.field);

    this.configurationForm = new FormGroup({
      key: new FormControl('', Validators.required),
      value: new FormControl('', Validators.required),
    });
  }

  getConfigurations() {
    this.configsClients.getConfigs().subscribe(
      (configs) => (this.configurations = configs),
      (_) => (this.configurations = [])
    );
  }

  // Edit Configuration
  openEditDialog(config: ConfigModel) {
    this.isShowEditDialog = true;
    this.configurationForm.setValue(config);
  }

  hideEditDialog() {
    this.isShowEditDialog = false;
    this.configurationForm.reset();
  }

  onEdit() {
    const { key, value } = this.configurationForm.value;

    const model: ConfigModel = {
      key,
      value,
    };

    this.configsClients.updateConfig(key, model).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.getConfigurations();
          this.notifiactionService.success('Edit Config Successfully');
        } else {
          this.notifiactionService.success(result.error);
        }

        this.hideEditDialog();
      },
      (_) => {
        this.notifiactionService.success('Edit Config Falied. Please try again');
        this.hideEditDialog();
      }
    );
  }
}
