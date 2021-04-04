import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PanelMenuModule } from 'primeng/panelmenu';
import { MenuModule } from 'primeng/menu';
import { DataViewModule } from 'primeng/dataview';

import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { CalendarModule } from 'primeng/calendar';
import { SliderModule } from 'primeng/slider';
import { MultiSelectModule } from 'primeng/multiselect';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { ProgressBarModule } from 'primeng/progressbar';
import { InputTextModule } from 'primeng/inputtext';
import { FileUploadModule } from 'primeng/fileupload';
import { ToolbarModule } from 'primeng/toolbar';
import { RatingModule } from 'primeng/rating';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputNumberModule } from 'primeng/inputnumber';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { StepsModule } from 'primeng/steps';
import { CardModule } from 'primeng/card';
import { TooltipModule } from 'primeng/tooltip';
import { PrintComponent } from './components/print/print.component';
import { PanelModule } from 'primeng/panel';
import { HistoryDialogComponent } from './components/history-dialog/history-dialog.component';
import { ImportComponent } from './components/import/import.component';
import { NgxBarcodeModule } from 'ngx-barcode';

@NgModule({
  declarations: [PrintComponent, HistoryDialogComponent, ImportComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    PanelMenuModule,
    MenuModule,
    DataViewModule,
    TableModule,
    CalendarModule,
    SliderModule,
    DialogModule,
    MultiSelectModule,
    ContextMenuModule,
    DropdownModule,
    ButtonModule,
    ToastModule,
    InputTextModule,
    ProgressBarModule,
    FileUploadModule,
    ToolbarModule,
    RatingModule,
    MessagesModule,
    MessageModule,
    FormsModule,
    RadioButtonModule,
    InputNumberModule,
    ConfirmDialogModule,
    InputTextareaModule,
    ProgressSpinnerModule,
    StepsModule,
    CardModule,
    InputNumberModule,
    TooltipModule,
    PanelModule,
    NgxBarcodeModule,
  ],
  exports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    PanelMenuModule,
    MenuModule,
    DataViewModule,
    TableModule,
    CalendarModule,
    SliderModule,
    DialogModule,
    MultiSelectModule,
    ContextMenuModule,
    DropdownModule,
    ButtonModule,
    ToastModule,
    MessagesModule,
    MessageModule,
    InputTextModule,
    ProgressBarModule,
    FileUploadModule,
    ToolbarModule,
    RatingModule,
    RadioButtonModule,
    InputNumberModule,
    ConfirmDialogModule,
    InputTextareaModule,
    ProgressSpinnerModule,
    StepsModule,
    CardModule,
    InputNumberModule,
    TooltipModule,
    PanelModule,
    NgxBarcodeModule,
    PrintComponent,
    HistoryDialogComponent,
  ],
  providers: [],
})
export class SharedModule {}
