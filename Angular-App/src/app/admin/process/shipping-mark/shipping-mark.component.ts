import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ProductClients, ProductModel, ShippingMarkClients, ShippingMarkModel, ShippingRequestModel } from 'app/shared/api-clients/shipping-app.client';
import { TypeColumn } from 'app/shared/configs/type-column';
import { WidthColumn } from 'app/shared/configs/width-column';
import { HistoryDialogType } from 'app/shared/enumerations/history-dialog-type.enum';
import { NotificationService } from 'app/shared/services/notification.service';
import { PrintService } from 'app/shared/services/print.service';
import { SelectItem } from 'primeng/api';

@Component({
  templateUrl: './shipping-mark.component.html',
  styleUrls: ['./shipping-mark.component.scss'],
})
export class ShippingMarkComponent implements OnInit {
  title = 'Shipping Mark';

  shippingMarks: ShippingMarkModel[] = [];
  products: ProductModel[] = [];
  selectedShippingMark: ShippingMarkModel;
  selectItems: SelectItem[] = [];

  isShowDeleteDialog: boolean;
  currentSelectedShippingMark: ShippingMarkModel[] = [];
  isDeleteMany: boolean;
  shippingMarkForm: FormGroup;

  isEdit = false;
  isShowDialog = false;
  isShowDialogHistory = false;
  titleDialog = '';

  cols: any[] = [];
  fields: any[] = [];
  TypeColumn = TypeColumn;
  HistoryDialogType = HistoryDialogType;

  get quantityControl() {
    return this.shippingMarkForm.get('quantity');
  }

  get productControl() {
    return this.shippingMarkForm.get('productId');
  }

  get notesControl() {
    return this.shippingMarkForm.get('notes');
  }

  get customerControl() {
    return this.shippingMarkForm.get('customerId');
  }

  get revisionControl() {
    return this.shippingMarkForm.get('revision');
  }

  get cartonNumberControl() {
    return this.shippingMarkForm.get('cartonNumber');
  }

  constructor(
    public printService: PrintService,
    private shippingMarkClients: ShippingMarkClients,
    private productClients: ProductClients,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.cols = [
      { header: '', field: 'checkBox', width: WidthColumn.CheckBoxColumn, type: TypeColumn.CheckBoxColumn },
      { header: 'Id', field: 'identifier', width: WidthColumn.IdentityColumn, type: TypeColumn.IdentityColumn },
      { header: 'Sequence', field: 'sequence', width: WidthColumn.QuantityColumn, type: TypeColumn.NormalColumn },
      { header: 'Product Number', field: 'product', subField: 'productNumber', width: WidthColumn.NormalColumn, type: TypeColumn.SubFieldColumn },
      { header: 'Quantity', field: 'quantity', width: WidthColumn.QuantityColumn, type: TypeColumn.NormalColumn },
      { header: 'Status', field: 'status', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Revision', field: 'revision', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Notes', field: 'notes', width: WidthColumn.DescriptionColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated By', field: 'lastModifiedBy', width: WidthColumn.NormalColumn, type: TypeColumn.NormalColumn },
      { header: 'Updated Time', field: 'lastModified', width: WidthColumn.DateColumn, type: TypeColumn.DateColumn },
    ];

    this.fields = this.cols.map((i) => i.field);

    this.initForm();
    this.initDataSource();
    this.initProducts();
  }

  initForm() {
    this.shippingMarkForm = new FormGroup({
      id: new FormControl(0),
      prefix: new FormControl(''),
      productId: new FormControl(0, Validators.required),
      notes: new FormControl(''),
      revision: new FormControl('', [Validators.required]),
      quantity: new FormControl(0, [Validators.required, Validators.min(1)]),
      cartonNumber: new FormControl('', [Validators.required]),
      customerId: new FormControl('', [Validators.required]),
      createdBy: new FormControl(''),
      created: new FormControl(null),
      lastModifiedBy: new FormControl(''),
      lastModified: new FormControl(null),
    });
  }

  initProducts() {
    this.productClients.getProducts().subscribe(
      (i) => {
        this.products = i;
      },
      (_) => (this.products = [])
    );
  }

  _mapToSelectItems(shippingRequests: ShippingRequestModel[]): SelectItem[] {
    return shippingRequests.map((i) => ({
      value: i.id,
      label: `${i.identifier}`,
    }));
  }

  initShippingMarks(shippingRequestId: number) {}

  handleOnChange(selectedShippingRequestId) {
    this.initShippingMarks(selectedShippingRequestId);
  }

  initDataSource() {
    this.shippingMarkClients.getShippingMarks().subscribe(
      (i) => (this.shippingMarks = i),
      (_) => (this.shippingMarks = [])
    );
  }

  openCreateDialog() {
    this.shippingMarkForm.reset();
    this.titleDialog = 'Create Shipping Mark';
    this.isShowDialog = true;
    this.isEdit = false;
  }

  onCreate() {
    const model = this.shippingMarkForm.value as ShippingMarkModel;
    model.id = 0;

    this.shippingMarkClients.addShippingMark(model).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Create Shipping Mark Successfully');
          this.initDataSource();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Create Shipping Mark Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  onSubmit() {
    if (this.shippingMarkForm.invalid) {
      return;
    }

    this.isEdit ? this.onEdit() : this.onCreate();
  }

  openEditDialog(shippingMark: ShippingMarkModel) {
    this.isShowDialog = true;
    this.titleDialog = 'Create Shipping Mark';
    this.isEdit = true;
    this.shippingMarkForm.patchValue(shippingMark);
  }

  hideDialog() {
    this.isShowDialog = false;
    this.isShowDialogHistory = false;
  }

  onEdit() {
    const { id } = this.shippingMarkForm.value;

    this.shippingMarkClients.updateShippingRequest(id, this.shippingMarkForm.value).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Edit Shipping Mark Successfully');
          this.initDataSource();
        } else {
          this.notificationService.error(result?.error);
        }

        this.hideDialog();
      },
      (_) => {
        this.notificationService.error('Edit Shipping Mark Failed. Please try again');
        this.hideDialog();
      }
    );
  }

  openDeleteDialog(shippingMark: ShippingMarkModel) {
    this.shippingMarkClients.deleteShippingMarkAysnc(shippingMark.id).subscribe(
      (result) => {
        if (result && result.succeeded) {
          this.notificationService.success('Delete Shipping Mark Successfully');
          this.initDataSource();
        } else {
          this.notificationService.error(result?.error);
        }
      },
      (_) => {
        this.notificationService.error('Delete Shipping Mark Failed. Please try again');
      }
    );
  }

  onPrint() {
    this.printService.printDocument('shipping-mark');
  }

  openHistoryDialog() {
    this.isShowDialogHistory = true;
  }
}
