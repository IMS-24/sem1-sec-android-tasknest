import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { UserTodo } from '../entities/user-todo.entity';

@Injectable()
export class UserTodoService {
  constructor(
    @InjectRepository(UserTodo)
    private userTodoRepository: Repository<UserTodo>,
  ) {}

  // Assign a Todo to a User
  async assignTodoToUser(
    userId: number,
    todoId: number,
    isOwner: boolean = false,
  ): Promise<UserTodo> {
    const userTodo = this.userTodoRepository.create({
      user: { id: userId },
      todo: { id: todoId },
      isOwner,
    });
    return this.userTodoRepository.save(userTodo);
  }

  // Update the status of a Todo assigned to a User
  async updateStatus(userTodoId: number, status: string): Promise<UserTodo> {
    await this.userTodoRepository.update(userTodoId, { status });
    return this.userTodoRepository.findOne({ where: { userTodoId } });
  }

  // Get all Todos assigned to a specific User
  async findTodosByUser(userId: number): Promise<UserTodo[]> {
    return this.userTodoRepository.find({
      where: { user: { id: userId } },
      relations: ['todo'],
    });
  }
}
