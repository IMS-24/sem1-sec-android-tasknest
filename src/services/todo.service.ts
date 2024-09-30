import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { Todo } from '../entities/todo.entity';

@Injectable()
export class TodoService {
  constructor(
    @InjectRepository(Todo)
    private todosRepository: Repository<Todo>,
  ) {}

  findAll(): Promise<Todo[]> {
    return this.todosRepository.find();
  }

  findOne(id: number): Promise<Todo> {
    return this.todosRepository.findOne({ where: { id } });
  }

  create(todo: Partial<Todo>): Promise<Todo> {
    const newTodo = this.todosRepository.create(todo);
    return this.todosRepository.save(newTodo);
  }

  async update(id: number, updateData: Partial<Todo>): Promise<Todo> {
    await this.todosRepository.update(id, updateData);
    return this.findOne(id);
  }

  async remove(id: number): Promise<void> {
    await this.todosRepository.delete(id);
  }
}
