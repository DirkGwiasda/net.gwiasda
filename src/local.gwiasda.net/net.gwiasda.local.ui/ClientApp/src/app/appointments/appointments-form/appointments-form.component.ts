import { Component, Output, Input, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Appointment } from '../appointment';
import { MatDialogRef } from '@angular/material/dialog';
import { DateService } from '../../date-service';

@Component({
  selector: 'app-appointments-form',
  templateUrl: './appointments-form.component.html'
})
export class AppointmentsFormComponent implements OnInit {

  constructor(private dialogRef: MatDialogRef<AppointmentsFormComponent>, private dateService: DateService) { }

  @Input() appointment: Appointment = new Appointment();
  @Output() saved = new EventEmitter<void>();
  @Output() deleted = new EventEmitter<string>();

  time: string = '';

  ngOnInit() {

    console.log("input: " + this.appointment.date);
    this.time = this.dateService.renderTime(this.appointment.date);
    console.log("result: " + this.time);
  }

  

  async save() {

    this.handleTime();
    this.saved.emit();
    this.close();
  }
  async delete() {
    this.deleted.emit(this.appointment.id);
    this.close();
  }
  
  handleTime() {
    var data = this.time.split(":");
    if (data && data.length == 2) {
      var hours = parseInt(data[0]);
      var minutes = parseInt(data[1]);

      if (isNaN(hours) || isNaN(minutes))
        alert("invalid time");
      else {
        var date = new Date(this.appointment?.date);
        date.setHours(hours);
        date.setMinutes(minutes);
        this.appointment.date = date;
      }
    }
  }
  close() {
    this.dialogRef.close();
  }
}
