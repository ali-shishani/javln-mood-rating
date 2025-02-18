import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { RouterModule } from '@angular/router';
import {MatCardModule} from '@angular/material/card';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button'
import {MatInputModule} from '@angular/material/input'


import { routes } from './layout.routing';
import { LayoutComponent } from './layout.component';
import { HomeComponent } from 'src/app/pages/home/home.component';

@NgModule({
  declarations: [
    LayoutComponent,
    HomeComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [],
})
export class LayoutModule {}
