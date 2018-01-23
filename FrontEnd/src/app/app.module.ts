import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule,JsonpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';
import { ImageUploadModule } from "angular2-image-upload";
import { Ng2FilterPipeModule } from 'ng2-filter-pipe';


import { AppComponent } from './app.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { MessageSendComponent } from './message/message-send.component';
import { MessageReceivedComponent } from './message/message-received.component';
import { AppUserComponent } from './appUser/appUser.component';
import { CommentListComponent } from './comment/comment-list.component';
import { ComplaintsComponent } from './complaint/complaints.component';
import { ComplaintSend } from './complaintSend/complaint-send.component';
import { TopicComponent } from './topic/topic.component';
import { SubforumsComponent } from './subforum/subforums.component';
import { SubforumComponent } from './subforum/subforum.component';
import { FilterComponent } from './filter/filter.component';

import {MessageService} from './services/message.service';
import {AppUserService} from './services/appUser.service';
import {CommentService} from './services/comment.service';
import {ComplaintService} from './services/complaint.service';
import {TopicService} from './services/topic.service';
import {SubforumService} from './services/subforum.service';



const Routes = [
  {path: "register", component:RegisterComponent},
  {path: "login", component:LoginComponent},
  {path: "message-send", component:MessageSendComponent},
  {path: "message-received", component:MessageReceivedComponent},
  {path: "appUser", component: AppUserComponent},
  {path: "complaints", component: ComplaintsComponent},
  {path: "topic/:id/:subId", component: TopicComponent},
  {path: "home", component: SubforumsComponent},
  {path: "subforum/:id", component: SubforumComponent},
  {path: "filter", component: FilterComponent},
]

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    MessageSendComponent,
    MessageReceivedComponent,
    AppUserComponent,
    ComplaintsComponent,
    ComplaintSend,
    TopicComponent,
    SubforumsComponent,
    SubforumComponent,
    CommentListComponent,
    FilterComponent,

  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    JsonpModule,
    ImageUploadModule.forRoot(),
    Ng2FilterPipeModule,
    RouterModule.forRoot(Routes)
  ],
  providers: [MessageService,AppUserService,CommentService,ComplaintService,
  TopicService,SubforumService],
  bootstrap: [AppComponent]
})
export class AppModule { }
