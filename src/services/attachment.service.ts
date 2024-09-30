import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { Attachment } from '../entities/attachment.entity';

@Injectable()
export class AttachmentService {
  constructor(
    @InjectRepository(Attachment)
    private attachmentRepository: Repository<Attachment>,
  ) {}

  // Add a new attachment to a Todo
  async addAttachment(
    todoId: number,
    attachmentData: Partial<Attachment>,
  ): Promise<Attachment> {
    const newAttachment = this.attachmentRepository.create({
      ...attachmentData,
      todo: { id: todoId },
    });
    return this.attachmentRepository.save(newAttachment);
  }

  // Retrieve all attachments for a Todo
  async findAttachmentsByTodoId(todoId: number): Promise<Attachment[]> {
    return this.attachmentRepository.find({
      where: { todo: { id: todoId } },
    });
  }

  // Delete an attachment by ID
  async removeAttachment(attachmentId: number): Promise<void> {
    await this.attachmentRepository.delete(attachmentId);
  }
}
