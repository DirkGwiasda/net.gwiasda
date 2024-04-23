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
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'logs', component: LoggingComponent, pathMatch: 'full' },
      { path: 'appointments', component: AppointmentsComponent, pathMatch: 'full' },
    ])
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'de-DE' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
