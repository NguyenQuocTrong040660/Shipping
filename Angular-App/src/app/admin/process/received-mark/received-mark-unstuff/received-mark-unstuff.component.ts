import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-received-mark-unstuff',
  templateUrl: './received-mark-unstuff.component.html',
  styleUrls: ['./received-mark-unstuff.component.scss'],
})
export class ReceivedMarkUnstuffComponent implements OnInit {
  @Input() titleDialog = '';
  @Input() isShowDialog = false;
  @Input() receivedMark = null;

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
      id: [0],
      unstuffQuantity: [0, [Validators.required]],
    });
  }

  onSubmit() {
    this.submitEvent.emit(this.receivedMarkUnstuffForm.value);
  }
}
