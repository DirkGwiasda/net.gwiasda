import { Component, OnInit } from '@angular/core';
import { Appointment } from './appointment';
import { AppointmentsFormComponent } from './appointments-form/appointments-form.component';
import { AppointmentDataService } from './appointment-data.service';
import { MatDialog } from '@angular/material/dialog';
import { GuidService } from '../guid-service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css']
})
export class AppointmentsComponent {

  constructor(private dataService: AppointmentDataService, private guidService: GuidService, private dialog: MatDialog) { }

  appointment: Appointment = new Appointment();
  appointments: Appointment[] = [];
  renderedAppointments: Appointment[] = [];
  showRecurring: boolean = false;

  ngOnInit() {
    this.readAppointments();
  }
  getDay(date: Date): string {
    var d = new Date(date);

    const nmbr = d.getDay();

    const days = ['So', 'Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa'];
    return days[nmbr];
  }
  renderTime(appointment: Appointment): string {

    const date: Date = new Date(appointment.date);

    const hours: number = date.getHours();
    const minutes: number = date.getMinutes();

    if (hours === 0 && minutes === 0)
      return '';

    const formattedHours: string = hours < 10 ? `0${hours}` : `${hours}`;
    const formattedMinutes: string = minutes < 10 ? `0${minutes}` : `${minutes}`;

    return ` ${formattedHours}:${formattedMinutes} Uhr`;
  }
  add() {
    var appointment = new Appointment();
    appointment.id = this.guidService.generateGUID();
    appointment.who = '-';
    appointment.recurringType = '-';
    this.openForm(appointment);
  }
  edit(id: string) {
    if (!id) return;

    this.dataService.getAppointment(id).subscribe(appointment => {
      if(appointment)
        this.openForm(appointment);
    });
  }
  openForm(appointment: Appointment) {
    const dialogRef = this.dialog.open(AppointmentsFormComponent, { });

    this.appointment = appointment;

    dialogRef.componentInstance.appointment = this.appointment;
    dialogRef.componentInstance.saved.subscribe(() => {
      this.handleSaved();
    });
    dialogRef.componentInstance.deleted.subscribe((id: string) => {
      this.handleDeleted(id);
    });
  }

  async handleSaved() {
    if(!this.appointment.id || this.appointment.id === '')
      this.appointment.id = this.guidService.generateGUID();

    await this.dataService.write(this.appointment)
      .then(() => this.readAppointments());
  }

  async handleDeleted(id: string) {
    await this.dataService.delete(id)
      .then(() => this.readAppointments());
  }

  readAppointments() {
    this.dataService.getAppointmentsForTimespan(new Date(2024, 1, 1), new Date(2024, 12, 31)).subscribe(appointments => {
      this.appointments = appointments ?? [];
      this.renderAppointments();
    });
  }

  renderAppointments() {
    if (!this.showRecurring) {
      const filtered = this.appointments.filter(appointment => appointment.recurringType === '-');
      this.renderedAppointments = filtered;
      return;
    }
    this.renderedAppointments = this.appointments;
  }
}
