import {
  Column,
  Entity,
  JoinColumn,
  ManyToOne,
  PrimaryGeneratedColumn,
} from 'typeorm';
import { User } from './user.entity';
import { Todo } from './todo.entity';

@Entity('user_todos')
export class UserTodo {
  @PrimaryGeneratedColumn({ name: 'user_todo_id' })
  userTodoId: number;

  @ManyToOne(() => User, (user) => user.userTodos, { onDelete: 'CASCADE' })
  @JoinColumn({ name: 'user_id' })
  user: User;

  @ManyToOne(() => Todo, (todo) => todo.userTodos, { onDelete: 'CASCADE' })
  @JoinColumn({ name: 'todo_id' })
  todo: Todo;

  @Column({ type: 'boolean', default: false, nullable: false })
  isOwner: boolean;

  @Column({ type: 'text', default: 'pending', nullable: false })
  status: string;
}
