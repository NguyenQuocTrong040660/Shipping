import { HostListener } from '@angular/core';
import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { PrintService } from 'app/shared/services/print.service';

@Component({
  selector: 'app-print',
  templateUrl: './print.component.html',
  styleUrls: ['./print.component.scss'],
})
export class PrintComponent implements OnInit, OnChanges {
  @Input() printData: any;

  constructor(private printService: PrintService) {}

  @HostListener('window:afterprint')
  onafterprint() {
    document.title = 'Shipping Application';
  }

  ngOnChanges() {
    if (this.printService.isPrinting) {
      const currentDate = new Date().toISOString().split('T')[0].split('-').join('');

      const receivedMarkId = this.printData.receivedMark && this.printData.receivedMark.identifier ? this.printData.receivedMark.identifier : '';
      const shippingMarkId = this.printData.shippingMark && this.printData.shippingMark.identifier ? this.printData.shippingMark.identifier : '';
      const id = receivedMarkId + shippingMarkId;

      const sequence = this.printData.sequence ? this.printData.sequence : '';

      // document.title = currentDate + '_' + id + '_' + sequence;
    }
  }

  ngOnInit() {
    if (this.printService.isPrinting) {
      this.printService.onDataReady();
    }
  }
}
