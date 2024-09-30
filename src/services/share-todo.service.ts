import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { ShareTodo } from '../entities/share-todo.entity';

@Injectable()
export class ShareTodoService {
  constructor(
    @InjectRepository(ShareTodo)
    private shareTodoRepository: Repository<ShareTodo>,
  ) {}

  // Share a Todo with another user
  async shareTodoWithUser(
    todoId: number,
    userId: number,
    permission: string,
  ): Promise<ShareTodo> {
    const shareTodo = this.shareTodoRepository.create({
      todo: { id: todoId },
      user: { id: userId },
      permission,
    });
    return this.shareTodoRepository.save(shareTodo);
  }

  // Get all shared Todos for a user
  async findSharedTodosByUser(userId: number): Promise<ShareTodo[]> {
    return this.shareTodoRepository.find({
      where: { user: { id: userId } },
      relations: ['todo'],
    });
  }

  // Update permissions for a shared Todo
  async updatePermissions(
    shareId: number,
    permission: string,
  ): Promise<ShareTodo> {
    await this.shareTodoRepository.update(shareId, { permission });
    return this.shareTodoRepository.findOne({ where: { shareId } });
  }
}
