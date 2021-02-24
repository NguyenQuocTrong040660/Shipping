import { NotificationService } from 'app/shared/services/notification.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfigClients, ConfigModel } from 'app/shared/api-clients/shipping-app.client';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';

@Component({
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.scss'],
})
export class ConfigurationComponent implements OnInit {
  title = 'Configurations';

  configurations: ConfigModel[] = [];
  selectedConfiguration: ConfigModel[];
  isShowEditDialog: boolean;
  configurationForm: FormGroup;

  WidthColumn = WidthColumn;
  TypeColumn = TypeColumn;

  cols: any[] = [];
  colFields = [];

  get keyControl() {
    return this.configurationForm.get('key');
  }

  get descriptionsControl() {
    return this.configurationForm.get('descriptions');
  }

  get valueControl() {
    return this.configurationForm.get('value');
  }

  constructor(private configsClients: ConfigClients, private notifiactionService: NotificationService) { }

  ngOnInit() {
    this.getConfigurations();

    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Key', field: 'key', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Descriptions', field: 'descriptions', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Value', field: 'value', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];

    this.colFields = this.cols.map((i) => i.field);

    this.initForm();
  }

  initForm() {
    this.configurationForm = new FormGroup({
      key: new FormControl('', Validators.required),
      descriptions: new FormControl(''),
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
    const { key, value, descriptions } = this.configurationForm.value;

    const model: ConfigModel = {
      key,
      value,
      descriptions,
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
        this.notifiactionService.error('Edit Config Falied. Please try again');
        this.hideEditDialog();
      }
    );
  }
}
