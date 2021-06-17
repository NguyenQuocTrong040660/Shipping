import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReceivedMarkPrintingModel } from 'app/shared/api-clients/shipping-app/shipping-app.client';

@Component({
  selector: 'app-received-mark-unstuff',
  templateUrl: './received-mark-unstuff.component.html',
  styleUrls: ['./received-mark-unstuff.component.scss'],
})
export class ReceivedMarkUnstuffComponent implements OnInit {
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() selectedReceivedMarkPrinting: ReceivedMarkPrintingModel;
  @Input() receivedMarkPrintings: ReceivedMarkPrintingModel[] = [];

  @Output() submitEvent = new EventEmitter<any>();
  @Output() hideDialogEvent = new EventEmitter<any>();

  receivedMarkUnstuffForm: FormGroup;

  get unstuffQuantityControl() {
    return this.receivedMarkUnstuffForm.get('unstuffQuantity');
  }

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
  }

  hideDialog() {
    this.unstuffQuantityControl.reset();
    this.hideDialogEvent.emit();
  }

  initForm() {
    this.receivedMarkUnstuffForm = this.fb.group({
      receivedMarkPrintingId: [0],
      unstuffQuantity: [0, [Validators.required, Validators.min(1)]],
    });

    this.unstuffQuantityControl.valueChanges.subscribe((i) => {
      if (i > this.selectedReceivedMarkPrinting.quantity) {
        this.unstuffQuantityControl.setErrors({ quantityNotValid: true });
      }
    });
  }

  onSubmit() {
    this.receivedMarkUnstuffForm.patchValue({
      receivedMarkPrintingId: this.selectedReceivedMarkPrinting.id,
    });

    this.submitEvent.emit(this.receivedMarkUnstuffForm.value);
  }
}
