import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DataComponent } from './data/data.component';
import { MoedaComponent } from './moeda/moeda.component';

@NgModule({
  declarations: [DataComponent, MoedaComponent],
  imports: [CommonModule, FormsModule],
  exports: [DataComponent, MoedaComponent]
})
export class ControlsModule {}
