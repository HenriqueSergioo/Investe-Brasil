import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Routing } from '../pages/routing';
import { AuthGuard } from '../shared/auth.guard.service';
import { LayoutComponent } from './layout.component';

@NgModule({
  declarations: [LayoutComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
        path: '',
        component: LayoutComponent,
        canActivate: [AuthGuard],
        children: Routing
      }
    ])
  ]
})
export class LayoutModule {}
