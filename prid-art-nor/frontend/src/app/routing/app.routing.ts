import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from '../components/home/home.component';
import { UserListComponent } from '../components/userlist/userlist.component';
import { LoginComponent } from '../components/login/login.component';
import { SignupComponent} from '../components/signup/signup.component';

import { UnknownComponent } from '../components/unknown/unknown.component';
import { BoardsComponent } from '../components/boards/boards.component';
import { BoardComponent} from '../components/board/board.component';
import { AuthGuard } from '../services/auth.guard';
import { Role } from '../models/user';
import { RestrictedComponent } from '../components/restricted/restricted.component';



const appRoutes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },

  {
    path: 'users',
    component: UserListComponent,
    canActivate: [AuthGuard],
    data: { roles: [Role.Admin] }
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'signup',
    component: SignupComponent
  },
  { path: 'boards',
  component: BoardsComponent },
  { path: 'boards/:id', 
    component: BoardComponent },
  { path: 'restricted', component: RestrictedComponent },
  { path: '**', component: UnknownComponent },

];

export const AppRoutes = RouterModule.forRoot(appRoutes);