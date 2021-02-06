import { Routes } from '@angular/router';
import { AuthenticationGuard } from 'app/shared/guards/authentication.guard';
import { AuthorizeGuard } from 'app/shared/guards/authorize.guard';
import { AdminComponent } from './admin.component';

export const adminRoutes: Routes = [
    {
        path: '',
        component: AdminComponent,
        canActivate: [AuthenticationGuard],
        children: [
            {
                path: 'files-management',
                loadChildren: () =>
                    import('./pages/files-management/files-management.module').then((m) => m.FilesManagementModule),
                canActivate: [AuthorizeGuard],
            },
            {
                path: 'attachment-types-management',
                loadChildren: () =>
                    import('./pages/attachment-types-management/attachment-types-management.module').then(
                        (m) => m.AttachmentTypesManagementModule
                    ),
                canActivate: [AuthorizeGuard],
            },
            {
                path: 'product',
                loadChildren: () => import('./pages/product/product.module').then((m) => m.ProductModule),
                canActivate: [AuthorizeGuard],
            },
            {
                path: 'product-type',
                loadChildren: () => import('./pages/product-type/product-type.module').then((m) => m.ProductTypeModule),
                canActivate: [AuthorizeGuard],
            },
            {
                path: 'country',
                loadChildren: () => import('./pages/country/country.module').then((m) => m.CountryModule),
                canActivate: [AuthorizeGuard],
            },
            {
                path: '',
                pathMatch: 'full',
                redirectTo: '/',
            },
        ],
    },
    {
        path: 'register',
        loadChildren: () => import('./pages/register/register.module').then((m) => m.RegisterModule),
    },
];
