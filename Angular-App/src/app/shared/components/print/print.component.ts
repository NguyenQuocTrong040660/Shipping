import { Component, Input, OnInit } from '@angular/core';
import { PrintService } from 'app/shared/services/print.service';

@Component({
  selector: 'app-print',
  templateUrl: './print.component.html',
  styleUrls: ['./print.component.scss']
})
export class PrintComponent implements OnInit {
  @Input() title: string;

  constructor(private printService: PrintService) { }

  ngOnInit() {
    if (this.printService.isPrinting) {
      this.printService.onDataReady();
    }
  }
}
