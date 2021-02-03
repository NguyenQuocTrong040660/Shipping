import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import * as CustomEditor from '../../ckeditor/build/ckeditor';

import { ChangeEvent } from '@ckeditor/ckeditor5-angular';
import { UploadAdapter } from './config/UploadAdapter';
import { ProductOverviewService} from 'app/services/product-overview.service';

@Component({
  selector: 'app-ckeditor',
  templateUrl: './ckeditor.component.html',
  styleUrls: ['./ckeditor.component.scss'],
})
export class CkeditorComponent implements OnInit {
  @Input() content = '';
  @Output() eventOnChange = new EventEmitter<any>(null);

  config = {
    toolbar: {
      items: [
        'heading',
        '|',
        'bold',
        'italic',
        'link',
        'bulletedList',
        'numberedList',
        '|',
        'indent',
        'outdent',
        '|',
        'imageUpload',
        'blockQuote',
        'insertTable',
        'undo',
        'redo',
        'mediaEmbed',
        'exportWord',
        'fontSize',
        'fontFamily',
        'highlight',
        'fontColor',
        'horizontalLine',
        'specialCharacters',
        'todoList',
        'fontBackgroundColor',
      ],
    },
  };

  public Editor = CustomEditor;
  constructor(private service: ProductOverviewService) {}

  ngOnInit(): void {}

  onChange({ editor }: ChangeEvent) {
    if (editor) {
      const data = editor.getData();
      this.eventOnChange.emit(data);
    }
  }

  onReady(eventData) {
    const service = this.service;
    eventData.plugins.get('FileRepository').createUploadAdapter = function (loader) {
      return new UploadAdapter(loader, service);
    };
  }
}
