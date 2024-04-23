import { Component, Output, EventEmitter } from '@angular/core';
import { Appointment } from '../appointment';
import { AppointmentDataService } from '../appointment-data.service';

@Component({
  selector: 'app-appointments-form',
  templateUrl: './appointments-form.component.html',
  styleUrls: ['./appointments-form.component.css']
})
export class AppointmentsFormComponent {

  constructor(dataService: AppointmentDataService) { this.dataService = dataService; }

  @Output() saved = new EventEmitter<void>();

  dataService: AppointmentDataService;
  appointment: Appointment = new Appointment();
  time: string = '';

  async save() {

    this.handleTime();
    
    await this.dataService.write(this.appointment);
    this.saved.emit();
  }
  handleTime() {
    var data = this.time.split(":");
    if (data && data.length == 2) {
      var hours = parseInt(data[0]);
      var minutes = parseInt(data[1]);

      if (isNaN(hours) || isNaN(minutes))
        alert("invalid time");
      else {
        this.appointment?.timestamp?.setHours(hours);
        this.appointment?.timestamp?.setMinutes(minutes);
      }
    }
  }
}
