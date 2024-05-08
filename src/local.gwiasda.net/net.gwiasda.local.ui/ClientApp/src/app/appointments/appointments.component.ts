import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Appointment } from './appointment';
import { AppointmentsFormComponent } from './appointments-form/appointments-form.component';
import { AppointmentDataService } from './appointment-data.service';
import { MatDialog } from '@angular/material/dialog';
import { GuidService } from '../guid-service';
import { Observable } from 'rxjs';
import { DateService } from '../date-service';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css']
})
export class AppointmentsComponent implements OnInit {

  constructor(private dataService: AppointmentDataService, private guidService: GuidService, private dialog: MatDialog, private dateService: DateService) { }

  appointment: Appointment = new Appointment();
  appointments: Appointment[] = [];
  renderedAppointments: Appointment[] = [];
  showRecurring: boolean = true;
  showFrom: Date = new Date();
  showTo: Date = new Date();

  ngOnInit() {
    this.showFrom.setDate((new Date()).getDate() - 7);
    this.showTo.setDate((new Date()).getDate() + 28);
    this.readAppointments();
  }
  refresh(dates: Date[]) {
    this.showFrom = dates[0];
    this.showTo = dates[1];
    this.readAppointments();
  }
  getDay(date: Date): string {
    var d = new Date(date);

    const nmbr = d.getDay();

    const days = ['So', 'Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa'];
    return days[nmbr];
  }
  renderDate(date: Date): string {

    var d = new Date(date);
    var result = this.dateService.renderDate(d);
    if (d.getUTCHours() != 0 || d.getUTCMinutes() != 0)
      result += ' ' + this.dateService.renderTime(d);

    return result;
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
    console.log(this.appointment);
    await this.dataService.write(this.appointment)
      .then(() => this.readAppointments());
  }

  async handleDeleted(id: string) {
    await this.dataService.delete(id)
      .then(() => this.readAppointments());
  }

  readAppointments() {
    console.log("readAppointments()");
    this.dataService.getAppointmentsForTimespan(this.showFrom, this.showTo).subscribe(appointments => {
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
