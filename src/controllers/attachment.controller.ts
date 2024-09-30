import { Body, Controller, Delete, Get, Param, Post } from '@nestjs/common';
import { AttachmentService } from '../services/attachment.service';
import { Attachment } from '../entities/attachment.entity';

@Controller('attachments')
export class AttachmentController {
  constructor(private readonly attachmentService: AttachmentService) {}

  @Post(':todoId')
  async addAttachment(
    @Param('todoId') todoId: number,
    @Body() attachmentData: Partial<Attachment>,
  ): Promise<Attachment> {
    return this.attachmentService.addAttachment(todoId, attachmentData);
  }

  @Get(':todoId')
  async findAttachmentsByTodoId(
    @Param('todoId') todoId: number,
  ): Promise<Attachment[]> {
    return this.attachmentService.findAttachmentsByTodoId(todoId);
  }

  @Delete(':attachmentId')
  async removeAttachment(
    @Param('attachmentId') attachmentId: number,
  ): Promise<void> {
    return this.attachmentService.removeAttachment(attachmentId);
  }
}
