import { Component } from '@angular/core';

@Component({
  selector: 'app-fima-booking',
  templateUrl: './fima.booking.component.html'
})
export class FiMaBookingComponent {

  activeFrame: string = 'create';

  setActiveFrame(frame: string) {
    this.activeFrame = frame;
  }
}
