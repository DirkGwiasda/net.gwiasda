import { CommonModule, CurrencyPipe } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MaterialModule } from './material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LoggingComponent } from './logging/logging.component';
import { LoggingAppSelectionComponent } from './logging/logging-app-selection/logging-app-selection.component';
import { LoggingEntriesComponent } from './logging/logging-entries/logging-entries.component';
import { LogEntryDetailsComponent } from './logging/logging-entries/logentry-details/logentry-details.component';

import { AppointmentsComponent } from './appointments/appointments.component';
import { AppointmentsFormComponent } from './appointments/appointments-form/appointments-form.component';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { FiMaComponent } from './fima/fima.component';
import { FiMaCategoryFormComponent } from './fima/fima-categories/fima-category-form/fima-category-form.component';
import { FiMaCategorySelectionComponent } from './fima/fima-categories/fima-category-selection/fima-category-selection.component';
import { FiMaCategoriesComponent } from './fima/fima-categories/fima.categories.component';
import { FiMaBookingComponent } from './fima/fima-booking/fima.booking.component';
import { FiMaBookingFormComponent } from './fima/fima-booking/fima-booking-form/fima-booking-form.component';
import { FiMaBookingOverviewComponent } from './fima/fima-booking/fima-booking-overview/fima-booking-overview.component';
import { FiMaRecurringBookingOverviewComponent } from './fima/fima-booking/fima-recurring-booking-overview/fima-recurring-booking-overview.component';
import { FiMaReportComponent } from './fima/fima-reports/fima.report.component';
import { FiMaCategoryReportComponent } from './fima/fima-reports/fima-category-report/fima-category-report.component';
import { FiMaCategoryReportBookingsComponent } from './fima/fima-reports/fima-category-report-bookings/fima-category-report-bookings.component';
import { LinksComponent } from './links/links.component';
import { LinkFormComponent } from './links/link-form/link-form.component';

import { LOCALE_ID } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import localeDe from '@angular/common/locales/de';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

registerLocaleData(localeDe);

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoggingComponent,
    LoggingAppSelectionComponent,
    LoggingEntriesComponent,
    LogEntryDetailsComponent,
    AppointmentsComponent,
    AppointmentsFormComponent,
    FiMaComponent,
    FiMaCategoryFormComponent,
    FiMaCategorySelectionComponent,
    FiMaCategoriesComponent,
    FiMaBookingComponent,
    FiMaBookingFormComponent,
    FiMaBookingOverviewComponent,
    FiMaRecurringBookingOverviewComponent,
    FiMaReportComponent,
    FiMaCategoryReportComponent,
    FiMaCategoryReportBookingsComponent,
    LinksComponent,
    LinkFormComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    BrowserAnimationsModule,
    MatProgressSpinnerModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'logs', component: LoggingComponent, pathMatch: 'full' },
      { path: 'appointment', component: AppointmentsComponent, pathMatch: 'full' },
      { path: 'fima', component: FiMaComponent, pathMatch: 'full' },
    ])
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'de-DE' },
    { provide: LOCALE_ID, useValue: 'de' },
    [CurrencyPipe]
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
