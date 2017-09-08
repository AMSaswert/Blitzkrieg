import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule,JsonpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';


import { AppComponent } from './app.component';
import { MessageComponent } from './message/message.component';
import { AppUserComponent } from './appUser/appUser.component';
import { CommentComponent } from './comment/comment.component';
import { ComplaintComponent } from './complaint/complaint.component';
import { TopicComponent } from './topic/topic.component';

import {MessageService} from './services/message.service';
import {AppUserService} from './services/appUser.service';
import {CommentService} from './services/comment.service';
import {ComplaintService} from './services/complaint.service';
import {TopicService} from './services/topic.service';



const Routes = [
  {path: "message", component: MessageComponent},
  {path: "appUser", component: AppUserComponent},
  {path: "comment", component: CommentComponent},
  {path: "complaint", component: ComplaintComponent},
  {path: "topic", component: TopicComponent},
]

@NgModule({
  declarations: [
    AppComponent,
    MessageComponent,
    AppUserComponent,
    CommentComponent,
    ComplaintComponent,
    TopicComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    JsonpModule,
    RouterModule.forRoot(Routes)
  ],
  providers: [MessageService,AppUserService,CommentService,ComplaintService,
  TopicService],
  bootstrap: [AppComponent]
})
export class AppModule { }
