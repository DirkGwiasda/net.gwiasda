import { Component, Output, Input, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { Appointment } from '../appointment';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-appointments-form',
  templateUrl: './appointments-form.component.html'
})
export class AppointmentsFormComponent {

  constructor(private dialogRef: MatDialogRef<AppointmentsFormComponent>) { }

  @Input() appointment: Appointment = new Appointment();
  @Output() saved = new EventEmitter<void>();
  @Output() deleted = new EventEmitter<string>();

  time: string = '00:00';

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
        this.appointment?.date?.setHours(hours);
        this.appointment?.date?.setMinutes(minutes);
      }
    }
  }
  close() {
    this.dialogRef.close();
  }
}
