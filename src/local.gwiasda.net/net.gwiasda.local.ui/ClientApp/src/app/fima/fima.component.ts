import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-fima',
  templateUrl: './fima.component.html'
})
export class FiMaComponent {

  activeFrame: string = 'reports';

  setActiveFrame(frame: string) {
    this.activeFrame = frame;
  }
}
