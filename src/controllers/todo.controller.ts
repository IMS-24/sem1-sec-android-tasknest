import {
  Body,
  Controller,
  Delete,
  Get,
  Param,
  Patch,
  Post,
} from '@nestjs/common';
import { TodoService } from '../services/todo.service';
import { Todo } from '../entities/todo.entity';

@Controller('todos')
export class TodoController {
  constructor(private readonly todoService: TodoService) {}

  @Get()
  async findAll(): Promise<Todo[]> {
    return this.todoService.findAll();
  }

  @Get(':id')
  async findOne(@Param('id') id: number): Promise<Todo> {
    return this.todoService.findOne(id);
  }

  @Post()
  async create(@Body() todoData: Partial<Todo>): Promise<Todo> {
    return this.todoService.create(todoData);
  }

  @Patch(':id')
  async update(
    @Param('id') id: number,
    @Body() updateData: Partial<Todo>,
  ): Promise<Todo> {
    return this.todoService.update(id, updateData);
  }

  @Delete(':id')
  async remove(@Param('id') id: number): Promise<void> {
    return this.todoService.remove(id);
  }
}
