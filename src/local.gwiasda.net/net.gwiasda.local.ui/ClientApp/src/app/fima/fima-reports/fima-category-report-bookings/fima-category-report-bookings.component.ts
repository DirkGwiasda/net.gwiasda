import { Component, Input, OnInit } from '@angular/core';
import { CategoryReport } from '../categoryReport';
import { Booking } from '../../fima-booking/booking';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-fima-category-selection',
  templateUrl: './fima-category-report-bookings.component.html'
})
export class FiMaCategoryReportBookingsComponent implements OnInit {

  constructor(private dialogRef: MatDialogRef<FiMaCategoryReportBookingsComponent>) { }

  ngOnInit(): void {
    if (this.categoryReport?.bookings) {
      this.dataSource = new MatTableDataSource<Booking>(this.categoryReport?.bookings);
    }
    
  }

  @Input() categoryReport: CategoryReport | undefined;

  dataSource: MatTableDataSource<Booking> = new MatTableDataSource<Booking>();
  displayedColumns: string[] = ['date', 'name', 'amount'];

  
  close() {
    this.dialogRef.close();
  }
}
