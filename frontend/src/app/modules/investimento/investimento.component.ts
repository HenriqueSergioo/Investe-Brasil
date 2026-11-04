import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-investimento',
  standalone: true,
  imports: [RouterOutlet],
  template: '<section class="page"><router-outlet></router-outlet></section>'
})
export class InvestimentoComponent {}
