import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './component/home/home.component';
import { RecordsComponent } from './component/records/records.component';
import { ErrorComponent } from './component/error/error.component';
import { AuthorizationGuardService } from './service/authorization/authorization.guard.service';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'home' },
  { path: 'home', component: HomeComponent },
  { path: 'records', component: RecordsComponent, canActivate: [AuthorizationGuardService] },
  { path: '**', component: ErrorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
