import {
  Column,
  Entity,
  JoinColumn,
  ManyToOne,
  PrimaryGeneratedColumn,
} from 'typeorm';
import { Todo } from './todo.entity';

@Entity('todo_attachments')
export class Attachment {
  @PrimaryGeneratedColumn({ name: 'attachment_id' })
  attachmentId: number;

  @ManyToOne(() => Todo, (todo) => todo.attachments, { onDelete: 'CASCADE' })
  @JoinColumn({ name: 'todo_id' })
  todo: Todo;

  @Column({ type: 'text', nullable: false })
  attachmentType: string;

  @Column({ type: 'text', nullable: false })
  attachmentBlob: string; // Storing as URL for better scalability.
}
