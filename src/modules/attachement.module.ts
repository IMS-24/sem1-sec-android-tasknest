import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Attachment } from '../entities/attachment.entity';
import { Todo } from '../entities/todo.entity';
import { AttachmentController } from '../controllers/attachment.controller';
import { AttachmentService } from '../services/attachment.service';

@Module({
  imports: [TypeOrmModule.forFeature([Attachment, Todo])],
  controllers: [AttachmentController],
  providers: [AttachmentService],
})
export class AttachmentModule {}
