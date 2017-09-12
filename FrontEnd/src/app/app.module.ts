import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule,JsonpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';


import { AppComponent } from './app.component';
import { MessageComponent } from './message/message-send.component';
import { AppUserComponent } from './appUser/appUser.component';
import { CommentComponent } from './comment/comment.component';
import { ComplaintsComponent } from './complaint/complaints.component';
import { TopicsComponent } from './topic/topics.component';
import { TopicComponent } from './topic/topic.component';
import { SubforumsComponent } from './subforum/subforums.component';
import { SubforumComponent } from './subforum/subforum.component';

import {MessageService} from './services/message.service';
import {AppUserService} from './services/appUser.service';
import {CommentService} from './services/comment.service';
import {ComplaintService} from './services/complaint.service';
import {TopicService} from './services/topic.service';
import {SubforumService} from './services/subforum.service';



const Routes = [
  {path: "message-send", component:MessageComponent},
  {path: "appUser", component: AppUserComponent},
  {path: "comment", component: CommentComponent},
  {path: "complaints", component: ComplaintsComponent},
  {path: "topics", component: TopicsComponent},
  {path: "topic/:id", component: TopicComponent},
  {path: "subforums", component: SubforumsComponent},
  {path: "subforum/:id", component: SubforumComponent},
]

@NgModule({
  declarations: [
    AppComponent,
    MessageComponent,
    AppUserComponent,
    CommentComponent,
    ComplaintsComponent,
    TopicsComponent,
    TopicComponent,
    SubforumsComponent,
    SubforumComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    JsonpModule,
    RouterModule.forRoot(Routes)
  ],
  providers: [MessageService,AppUserService,CommentService,ComplaintService,
  TopicService,SubforumService],
  bootstrap: [AppComponent]
})
export class AppModule { }
