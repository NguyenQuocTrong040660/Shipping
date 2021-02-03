import { Component, OnInit } from '@angular/core';
import { AlbumService } from '@grx/core';
import { AttachmentTypeDto } from 'app/api-clients/album-client';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ProductOverviewService } from 'app/services/product-overview.service';
import { ProductTypeService } from 'app/services/product-type.service';
import { ProductService } from 'app/services/product.service';
import { BrandService } from 'app/services/brand.service';
import { ProductType, ShippingAppClients, Country, Brand, ProductOverview, FileParameter } from 'app/api-clients/shippingapp-client';
import { StateService, States } from 'app/services/state.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { company } from './../../../../../../src/assets/company';
import { takeUntil } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { from, Subject } from 'rxjs';
declare var $: any;
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
  providers: [MessageService, ConfirmationService],
})
export class ProductComponent implements OnInit {
  id = '';
  editMode = false;
  submitted = false;
  form: FormGroup;
  productOverviews: ProductOverview[] = [];
  productType: ProductType[] = [];
  country: Country[] = [];
  brand: Brand[] = [];
  company = company;
  companyIndexState: number;
  titleDialog: string;
  dialog = false;
  companyDisabled: '';

  get productName() {
    return this.form.get('productName');
  }

  get highLevelDesc() {
    return this.form.get('highLevelDesc');
  }

  get mediumLevelDesc() {
    return this.form.get('mediumLevelDesc');
  }

  get normalLevelDesc() {
    return this.form.get('normalLevelDesc');
  }

  get imageUrl() {
    return this.form.get('imageUrl');
  }

  get imageName() {
    return this.form.get('imageName');
  }
  get productTypeId() {
    return this.form.get('productTypeId');
  }

  get countryCode() {
    return this.form.get('countryCode');
  }

  get brandId() {
    return this.form.get('brandId');
  }

  get companyIndex() {
    return this.form.get('companyIndex');
  }
  get hightlightProduct() {
    return this.form.get('hightlightProduct');
  }
  private destroyed$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private services: AlbumService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private stateService: StateService,
    private productTypeService: ProductTypeService,
    private productService: ProductService,
    private brandService: BrandService,
    private productOverViewService: ProductOverviewService,
    private router: Router
  ) { }

  ngOnInit() {
    this.stateService.selectState(companyIndex => {
      this.productOverViewService.getAllProductOverview(2).subscribe((data) => {
        this.productOverviews = data;
        this.companyIndexState = 2;
      });
    }).subscribe();

    this.getProductType();
    this.getCountry();
    this.getBrand();

    this.form = this.fb.group({
      id: ['00000000-0000-0000-0000-000000000000'],
      productName: ['', [Validators.required, Validators.maxLength(100)]],
      highLevelDesc: ['', Validators.maxLength(300)],
      mediumLevelDesc: [''],
      normalLevelDesc: [''],
      productTypeId: ['', [Validators.required]],
      countryCode: ['', [Validators.required]],
      brandId: ['', [Validators.required]],
      companyIndex: [''],
      hightlightProduct: [],
      imageUrl: [''],
      imageName: [''],
    });
  }

  getProductType() {
    this.productTypeService
      .getProductType(0)
      .pipe(takeUntil(this.destroyed$))
      .subscribe((data) => {
        this.productType = data;
      });
  }

  getCountry() {
    this.productService.getAllCountry()
      .pipe(takeUntil(this.destroyed$))
      .subscribe((data) => {
        this.country = data;
      });
  }

  getBrand() {
    this.brandService.getAllBrand(0)
      .pipe(takeUntil(this.destroyed$))
      .subscribe((data) => {
        this.brand = data;
      });
  }
  handleValueCkChange(value) {
    this.form.patchValue({
      mediumLevelDesc: value,
    });
  }

  reloadData() {
    this.productOverViewService.getAllProductOverview(this.companyIndexState).subscribe((data) => {
      this.productOverviews = data
    });
  }

  openNew() {
    this.form.reset();
    this.submitted = false;
    this.titleDialog = 'Thêm Sản Phẩm';
    this.editMode = false;
    this.companyDisabled = null;
    this.onChangeCompany(2);
    this.dialog = true;
  }

  hideDialog() {
    this.dialog = false;
    this.submitted = false;
    this.form.reset();
  }

  editProduct(id: string) {
    this.titleDialog = 'Cập Nhật Sản Phẩm';
    this.getProductById(id);
    this.editMode = true;
    this.companyDisabled = '';
    this.dialog = true;
  }


  getProductById(id) {
    this.productOverViewService
      .getProductOverViewById(id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe((result: ProductOverview) => {
        if (result) {
          // tslint:disable-next-line: max-line-length
          const { id, productName, highLevelDesc, mediumLevelDesc, normalLevelDesc, imageName, productTypeId, countryCode, brandId, imageUrl, companyIndex, hightlightProduct } = result;
          this.form.patchValue({
            id: id,
            productName,
            highLevelDesc,
            mediumLevelDesc,
            normalLevelDesc,
            imageUrl,
            imageName,
            productTypeId,
            countryCode,
            brandId,
            companyIndex,
            hightlightProduct,
            file: '',
          });
          this.productTypeService.getProductType(companyIndex).subscribe(data => {
            this.productType = data;
          });
          this.brandService.getAllBrand(companyIndex).subscribe(data => {
            this.brand = data;
          });
        } else {
          this.editMode = false;
          this.id = '00000000-0000-0000-0000-000000000000';
        }
      });
  }


  deleteProduct(productOverView: ProductOverview) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + productOverView.productName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        // this.attachmentTypes = this.attachmentTypes.filter((val) => val.id !== attachmentType.id);
        this.productOverViewService
          .deletedProductOverView(productOverView.id)
          .subscribe((result) =>
            this.showMessage('success', 'Successful', 'Xóa Sản Phẩm Thành Công!!!')
          );
        this.productOverViewService.getAllProductOverview(2).subscribe((data) => {
          this.productOverviews = data
        });
      },
    });
  }

  deleteSelectedItems() {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the selected items?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        // this.attachmentTypes = this.products.filter(val => !this.selectedProducts.includes(val));
        // this.selectedProducts = null;
        // this.messageService.add({severity:'success', summary: 'Successful', detail: 'Products Deleted', life: 3000});
      },
    });
  }

  showMessage(type: string, summary: string, detail: string = '', timeLife: number = 3000) {
    this.messageService.add({ severity: type, summary: summary, detail: detail, life: timeLife });
  }

  onFileChange(event) {
    const reader = new FileReader();

    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;

      reader.readAsDataURL(file);

      const fileToUpload: FileParameter = {
        data: file,
        fileName: file.name,
      };

      reader.onload = () => {
        this.form.patchValue({
          imageUrl: reader.result,
        });
      };

      this.productOverViewService.uploadImg(fileToUpload).subscribe(
        (result: any) => {
          if (result) {
            const { imageName, folderName, imageUrl } = result;
            this.form.patchValue({
              imageUrl: imageUrl,
            });

            this.form.patchValue({
              imageName: imageName,
            });
          }
        },
        (e) => {
          return '';
        }
      );
    }
  }

  onChangeCompany(value) {
    this.productType = null;
    this.brand = null;
    this.productTypeService.getProductType(value).subscribe(data => {
      this.productType = data;
    });
    this.brandService.getAllBrand(value).subscribe(data => {
      this.brand = data;
    });
  }

  onSubmit() {
    this.submitted = true;

    if (this.form.invalid) {
      return;
    }

    if (this.editMode) {
      this.handleUpdateProduct();
    } else {
      this.handleAddProduct();
    }
  }

  handleAddProduct() {
    this.form.value.id = '00000000-0000-0000-0000-000000000000';
    if (this.form.value.hightlightProduct == null) {
      this.form.value.hightlightProduct = false;
    }
    this.form.value.companyIndex = 2;
    this.productOverViewService.
      insertProductOverview(this.form.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result > 0) {
            this.showMessage('success', 'Successful', 'Thêm mới sản phẩm thành công!!!')
            this.hideDialog();
            this.reloadData();
          } else {
            this.showMessage('error', 'Failed', 'Thêm mới sản phẩm thất bại, Vui lòng kiểm tra lại!!!')
          }
        },
        (e) => {
          this.showMessage('error', 'Failed', 'Đã có lỗi xảy ra vui lòng thêm lại sau !!!')
        }
      );
  }

  handleUpdateProduct() {
    this.productOverViewService
      .updateProductOverView(this.form.value.id, this.form.value)
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (result) => {
          if (result > 0) {
            this.showMessage('success', 'Successful', 'Cập nhật sản phẩm thành công!!!')
            this.hideDialog();
            this.reloadData();
          } else {
            this.showMessage('success', 'Successful', 'Cập nhật sản phẩm thất bại!!!')
          }
        },
        (e) => {
          this.showMessage('error', 'Failed', 'Đã có lỗi xảy ra vui lòng thêm lại sau !!!')
        }
      );
  }
}
