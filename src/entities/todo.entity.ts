import {
  Column,
  Entity,
  ManyToOne,
  OneToMany,
  PrimaryGeneratedColumn,
} from 'typeorm';
import { UserTodo } from './user-todo.entity';
import { ShareTodo } from './share-todo.entity';
import { Attachment } from './attachment.entity';
import { User } from './user.entity';

@Entity('todos')
export class Todo {
  @PrimaryGeneratedColumn({ name: 'todo_id' })
  id: number;

  @Column({ type: 'text', nullable: false })
  name: string;

  @Column({ type: 'timestamp', nullable: true })
  dueDate: Date;

  @ManyToOne(() => User, (user) => user.userTodos, { onDelete: 'CASCADE' })
  creator: User;

  @Column({ type: 'text', nullable: true })
  location: string;

  @OneToMany(() => UserTodo, (userTodo) => userTodo.todo)
  userTodos: UserTodo[];

  @OneToMany(() => ShareTodo, (shareTodo) => shareTodo.todo)
  sharedUsers: ShareTodo[];

  @OneToMany(() => Attachment, (attachment) => attachment.todo)
  attachments: Attachment[];
}
