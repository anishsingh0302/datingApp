import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../models/User';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model : any = {}
  //loggedIn : boolean;
  // currentUser$ : Observable<User>;

  constructor(public accountService : AccountService, private route : Router, private toastr : ToastrService) { }

  ngOnInit(): void {
   // this.currentUser$ = this.accountService.currentUser$;
    //this.getCurrentUser();
  }

  logOut()
  {
    this.accountService.logout();
    this.route.navigateByUrl('/');
   // this.loggedIn = false;
  }

  // getCurrentUser() {
  //   this.accountService.currentUser$.subscribe( user => {
  //     this.loggedIn = !!user;
  //   }, error => {
  //     console.log(error);
  //   })
  // }

  login()
  {
    this.accountService.login(this.model).subscribe(response => {
      //console.log(response);
      //this.loggedIn = true;  
      this.route.navigateByUrl('/members');
    })
  // }, error => { 
  //    console.log(error);
  //    this.toastr.error(error.error);
  // })
    
  }

}
