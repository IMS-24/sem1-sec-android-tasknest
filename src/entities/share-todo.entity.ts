import {
  Column,
  Entity,
  JoinColumn,
  ManyToOne,
  PrimaryGeneratedColumn,
} from 'typeorm';
import { User } from './user.entity';
import { Todo } from './todo.entity';

@Entity('todo_shares')
export class ShareTodo {
  @PrimaryGeneratedColumn({ name: 'share_id' })
  shareId: number;

  @ManyToOne(() => Todo, (todo) => todo.sharedUsers, { onDelete: 'CASCADE' })
  @JoinColumn({ name: 'todo_id' })
  todo: Todo;

  @ManyToOne(() => User, (user) => user.sharedTodos, { onDelete: 'CASCADE' })
  @JoinColumn({ name: 'user_id' })
  user: User;

  @Column({ type: 'text', nullable: false })
  permission: string; // E.g., 'read', 'write', etc.
}
