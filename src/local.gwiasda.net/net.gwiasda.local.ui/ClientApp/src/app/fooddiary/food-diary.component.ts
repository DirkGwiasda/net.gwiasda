import { Component } from '@angular/core';

@Component({
  selector: 'app-food-diary.component',
  templateUrl: './food-diary.component.html'
})
export class FoodDiaryComponent {

  activeFrame: string = 'create';

  setActiveFrame(frame: string) {
    this.activeFrame = frame;
  }
}
