import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'app/shared/services/notification.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfigClients, ConfigModel } from 'app/shared/api-clients/shipping-app/shipping-app.client';
import { WidthColumn } from 'app/shared/configs/width-column';
import { TypeColumn } from 'app/shared/configs/type-column';
import { Subject } from 'rxjs';

@Component({
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.scss'],
})
export class ConfigurationComponent implements OnInit, OnDestroy {
  title = 'Configuration';

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

  private destroyed$ = new Subject<void>();

  constructor(private configsClients: ConfigClients, private notifiactionService: NotificationService) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Key', field: 'key', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Descriptions', field: 'descriptions', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Value', field: 'value', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];

    this.colFields = this.cols.map((i) => i.field);

    this.getConfigurations();
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
    this.configsClients
      .getConfigs()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (configs) => (this.configurations = configs),
        (_) => (this.configurations = [])
      );
  }

  openEditDialog(config: ConfigModel) {
    this.isShowEditDialog = true;
    this.configurationForm.patchValue(config);
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

    this.configsClients
      .updateConfig(key, model)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result && result.succeeded) {
            this.notifiactionService.success('Edit Config Successfully');
            this.getConfigurations();
            this.hideEditDialog();
          } else {
            this.notifiactionService.error(result.error);
          }
        },
        (_) => {
          this.notifiactionService.error('Edit Config Falied. Please try again later');
          this.hideEditDialog();
        }
      );
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
