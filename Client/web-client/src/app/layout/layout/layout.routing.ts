import { Routes } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { HomeComponent } from 'src/app/pages/home/home/home.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: '',
        component: HomeComponent
      },
    ],
  },
];

export const components = [LayoutComponent];
