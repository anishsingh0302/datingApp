import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr/toastr/toastr.service';
import { take } from 'rxjs/operators';
import { Member } from '../models/member';
import { User } from '../models/User';
import { AccountService } from '../_services/account.service';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  member : Member;
  user : User;
  
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }
  constructor(private AccountService: AccountService, private memberService : MembersService, private toastr: ToastrService) {
    this.AccountService.currentUser$.pipe(take(1)).subscribe(user => this.user == user);
   }

  ngOnInit(): void {
    this.loadmember();
  }

  loadmember()
  {
     this.memberService.getMember(this.user.UserName).subscribe(member => {
       this.member = member;
     })
  }

  updateMember() {
    //console.log(this.member);
   // this.toastr.success('Profile updated successfully');
     this.memberService.updateMember(this.member).subscribe(() => {
      this.toastr.success('Profile updated successfully');
      this.editForm.reset(this.member);
    })
  }

}
