import { Component, OnInit } from '@angular/core';
import { Appointment } from './appointment';
import { AppointmentDataService } from './appointment-data.service';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css']
})
export class AppointmentsComponent {

  constructor(dataService: AppointmentDataService) { this.dataService = dataService; }

  dataService: AppointmentDataService;
  appointments: Appointment[] = [];

  ngOnInit() {
    this.readAppointments();
  }

  async handleSaved() {
    await this.readAppointments();
  }

  async readAppointments(): Promise<void> {
    this.dataService.readAll()
      .then(appointments => {
        this.appointments = appointments ?? [];
      });
  }
  async delete(id: string) {
    console.log("delete: " + id);

    await this.dataService.delete(id);
    await this.readAppointments();
  }
}
